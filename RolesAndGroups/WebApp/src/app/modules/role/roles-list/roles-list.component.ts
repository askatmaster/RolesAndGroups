import {Component, inject, OnInit} from '@angular/core';
import {DestroyComponent} from "../../../common-components/destroy/destroy.component";
import {ClientApi, GroupDto, RoleDto} from "../../../api/client-api";
import {BehaviorSubject, Observable, Subject, switchMap, takeUntil, tap} from "rxjs";
import {FormBuilder, FormGroup, Validators} from '@angular/forms';

@Component({
  selector: 'app-roles-list',
  //webpack требует такое подключение шаблона
  template: require('./roles-list.component.html').default,
  styleUrls: ['./roles-list.component.css']
})
export class RolesListComponent extends DestroyComponent implements OnInit {
  isLoading = false;
  clientApi: ClientApi; //класс для отправки запросов на бэк
  selectedRole: RoleDto | undefined; //выбранная в данный момент роль
  selectedGroups = new Array<GroupDto>(); //выбранные в данный момент роли
  isAvailable: boolean = true //этот флаг нужен для переключения шаблона модального окна
  roles$: Observable<RoleDto[]>; //наблюдаемые роли
  rolesSbj = new BehaviorSubject<RoleDto[] | null>(null) //субьект для обновления списка ролей при добавлении и удалении
  errorSbj = new Subject<string>() // субьект для отображения ошибок
  formBuilder: FormBuilder // рективные формы
  dataForm: FormGroup | undefined; // рективные формы для редактирования роли
  allGroups = new Array<GroupDto>() // все подгруженные группы
  filteredGroups = new Array<GroupDto>() // отфильтрованные группы отображаемые на панели
  totalPages = new Array<number>(); // общее количество страниц для отображения пагинации
  filter = 'all' // дефолтный фильтр отображаемых групп
  modalData: any = {  // данные для модального окна. Содержимое модального окна зависит от переданных ему данных
    formControls: {
      name: ['', [Validators.required]],
    },
    formControlsNames: [{
      label: 'Name',
      name: 'name'
    }]
  }

  constructor() {
    super();
    this.clientApi = inject(ClientApi); // т.к. webpack 5 ломает DI контейнер ангуляра, приходится инжектить сервисы напрямую, а не через параметры конструктора
    this.formBuilder = inject(FormBuilder); // инжектим формбилдер
    this.roles$ = this.rolesSbj // выстраиваем пайплайн для субьекта ролей
      .pipe(switchMap(() => this.clientApi.getAllRoles().pipe(tap(roles => this.initialSelectRole(roles)))))
    this.getPagedGroups(1)

    this.dataForm = this.formBuilder.group({ //создаем форму
      id: [''],
      name: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
  }

  //при смене страницы подгружаем новые данные
  getPagedGroups(page: number): void {
    this.totalPages = []
    this.clientApi.getPagedGroups(page).pipe(takeUntil(this.destroyed$)).subscribe(page => {
      //создаем массив количества страниц
      if (page.items && page.totalPages) {
        for (let i = 1; i <= page.totalPages; i++) {
          this.totalPages.push(i)
        }

        //обновляем массив всех групп и массив отфильтрованных групп
        this.allGroups = page.items
        this.filteredGroups = page.items

        //вызываем метод выбора роли чтобы отметить выбранные группы для новой страницы
        if(this.selectedRole)
          this.selectRole(this.selectedRole)
      }

      //сбрасываем фильтр на дефолтный
      this.filter = 'all'
    })
  }

  //при загрузке или обновлении страницы инициализируем выбор первой роли из списка
  initialSelectRole(roles: RoleDto[]) {
    if (roles.length > 0 && !this.selectedRole) {
      this.selectRole(roles[0])
    }
  }

  //выбор определенной роли.
  selectRole(role: RoleDto): void {
    this.selectedRole = role

    //обновляем массив выбранных групп
    if (this.selectedRole.groups) {
      this.selectedGroups = []
      this.selectedGroups = this.selectedGroups.concat(this.selectedRole.groups)
    }

    //обнуляем список отфильтрованных групп и спусти указанны таймаут заного инициализируем
    this.filteredGroups = []
    setTimeout(() => {
      this.filteredGroups = this.allGroups
      this.markSelectedGroups(role)
    }, 1);
    this.dataForm?.patchValue(role)
  }

  //маркируем группы выбранные отмеченные для этой роли
  markSelectedGroups(role: RoleDto) {
    for (let i = 0; i < this.filteredGroups.length; ++i) {
      let group = this.filteredGroups[i]
      group.isSelected = !!role?.groups?.filter(x => x.id === group.id)[0];
    }
  }

  //узнаем была ли уже отмечена группа
  isCheckedSelectedGroups(groups: GroupDto[], group: GroupDto) {
    return !!groups.filter(x => x.id === group.id)[0];
  }

  //выбираем группу
  selectGroup(group: GroupDto, selectedRole:RoleDto) {

    if (this.isCheckedSelectedGroups(this.selectedGroups, group)) {
      // если группа на которую кликнули уже была отмечена
      // значит группу исключили из списка, находим индекс этой группы и удаляем ее из массива
      let selectedGroup = this.selectedGroups.filter(x => x.id === group.id)[0]
      const index = this.selectedGroups.indexOf(selectedGroup);
      if (index > -1) {
        this.selectedGroups.splice(index, 1);

        //при смене страницы сохраняем или удаляем группу у выбранной роли
        if(selectedRole.groups){
          let groupInSelectedRole = selectedRole.groups.filter(x => x.id === group.id)[0]
          const groupInSelectedRoleIndex = selectedRole.groups.indexOf(groupInSelectedRole);
          selectedRole.groups?.splice(groupInSelectedRoleIndex, 1);
        }
      }
    } else {
      // если группа на которую кликнули еще не была отмечена
      // значит группу добавили в список, добавляем группу в список выбранных групп и в спислк групп выбранной роли на случай смены пагинации
      this.selectedGroups.push(group)
      selectedRole.groups?.push(group)
    }
  }

  //создать роль
  createRole(data: RoleDto): void {
    this.isLoading = true
    this.clientApi.createRole(data)
      .subscribe({
        next: () => {
          this.isLoading = false
          //после создания роли обновляем список ролей
          this.rolesSbj.next(null)
        }
      })

    //перезагрузка модального окна
    //изза запрета использовать css фреймворки у меня возникли пробелы с обработкой модального окна на чистом css
    //поэтому пришлось прибегнуть к такому решению
    this.reloadModalComponent()
  }

  //удалить роль
  deleteRole(roleId: number): void {
    this.isLoading = true
    this.clientApi.deleteRole(roleId)
      .subscribe({
        next: () => {
          this.isLoading = false
          //после удаления роли обновляем список ролей
          this.rolesSbj.next(null)
        }
      })
  }

  //редактирование роли
  submit() {
    this.isLoading = true
    if (this.dataForm?.invalid) {
      this.errorSbj.next("Введите корректные данные")
      return;
    }

    let role = this.dataForm?.value as RoleDto
    role.groups = this.selectedGroups // заменяем список групп роли на список выбранных на панели групп
    this.clientApi.editRole(role)
      .subscribe({
        next: () => {
          this.isLoading = false
          this.rolesSbj.next(null)
        }
      })
  }

  //поиск
  onSearch(search: string) {
    this.filteredGroups = this.allGroups.filter(group => search.toLowerCase() && group.name?.toLowerCase().includes(search))

    if(search == ''){
      this.filteredGroups = this.allGroups
    }
  }

  //установить фильтр
  setFilter(filter: any) {
    this.filteredGroups = this.allGroups
    switch (filter) {
      case 'all':
        this.filteredGroups = this.allGroups
        break
      case 'isNotSelected':
        this.filteredGroups = this.filteredGroups.filter(group => group.isSelected === false)
        break
      case 'isSelected':
        this.filteredGroups = this.filteredGroups.filter(group => group.isSelected === true)
        break
    }
    this.filter = filter
  }

  //перезагрузка модального окна спустя таймаут
  reloadModalComponent() {
    this.isAvailable = false

    setTimeout(() => {
      this.isAvailable = true
    }, 10);
  }
}

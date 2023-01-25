import {Component, inject, OnInit} from '@angular/core';
import {DestroyComponent} from "../../../common-components/destroy/destroy.component";
import {ClientApi, GroupDto, RoleDto} from "../../../api/client-api";
import {BehaviorSubject, Observable, Subject, switchMap, takeUntil, tap} from "rxjs";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";

@Component({
  selector: 'app-groups-list',
  //webpack требует такое подключение шаблона
  template: require('./groups-list.component.html').default,
  styleUrls: ['./groups-list.component.css']
})
export class GroupsListComponent extends DestroyComponent implements OnInit {
  isLoading = false;
  clientApi: ClientApi; //класс для отправки запросов на бэк
  selectedGroup: GroupDto | undefined; //выбранная в данный момент группа
  selectedRoles = new Array<RoleDto>(); //выбранные в данный момент роли
  isAvailable: boolean = true //этот флаг нужен для переключения шаблона модального окна
  groups$: Observable<GroupDto[]>; //наблюдаемые группы
  groupsSbj = new BehaviorSubject<GroupDto[] | null>(null) //субьект для обновления списка групп при добавлении и удалении
  errorSbj = new Subject<string>() // субьект для отображения ошибок
  formBuilder: FormBuilder // рективные формы
  dataForm: FormGroup | undefined; // рективные формы для редактирования группы
  allRoles = new Array<RoleDto>() // все подгруженные ролей
  filteredRoles = new Array<RoleDto>() // отфильтрованные роли отображаемые на панели
  totalPages = new Array<number>(); // общее количество страниц для отображения пагинации
  filter = 'all' // дефолтный фильтр отображаемых ролнй
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
    this.groups$ = this.groupsSbj // выстраиваем пайплайн для субьекта групп
      .pipe(switchMap(() => this.clientApi.getAllGroups().pipe(tap(groups => this.initialSelectGroup(groups)))))
    this.getPagedRoles(1)

    this.dataForm = this.formBuilder.group({ //создаем форму
      id: [''],
      name: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
  }

  //при смене страницы подгружаем новые данные
  getPagedRoles(page: number): void {
    this.totalPages = []
    this.clientApi.getPagedRoles(page).pipe(takeUntil(this.destroyed$)).subscribe(page => {
      //создаем массив количества страниц
      if (page.items && page.totalPages) {
        for (let i = 1; i <= page.totalPages; i++) {
          this.totalPages.push(i)
        }

        //обновляем массив всех ролей и массив отфильтрованных ролей
        this.allRoles = page.items
        this.filteredRoles = page.items

        //вызываем метод выбора группы чтобы отметить выбранные роли для новой страницы
        if (this.selectedGroup)
          this.selectGroup(this.selectedGroup)
      }

      //сбрасываем фильтр на дефолтный
      this.filter = 'all'
    })
  }

  //при загрузке или обновлении страницы инициализируем выбор первой группы из списка
  initialSelectGroup(groups: GroupDto[]) {
    if (groups.length > 0 && !this.selectedGroup) {
      this.selectGroup(groups[0])
    }
  }

  //выбор определенной группы.
  selectGroup(group: GroupDto): void {
    this.selectedGroup = group

    //обновляем массив выбранных ролей
    if (this.selectedGroup.roles) {
      this.selectedRoles = []
      this.selectedRoles = this.selectedRoles.concat(this.selectedGroup.roles)
    }

    //обнуляем список отфильтрованных ролей и спусти указанны таймаут заного инициализируем
    this.filteredRoles = []
    setTimeout(() => {
      this.filteredRoles = this.allRoles
      this.markSelectedRoles(group)
    }, 1);
    this.dataForm?.patchValue(group)
  }

  //маркируем роли выбранные отмеченные для этой группы
  markSelectedRoles(group: GroupDto) {
    for (let i = 0; i < this.filteredRoles.length; ++i) {
      let role = this.filteredRoles[i]
      role.isSelected = !!group?.roles?.filter(x => x.id === role.id)[0];
    }
  }

  //узнаем была ли уже отмечена роль
  isCheckedSelectedRoles(roles: RoleDto[], role: RoleDto) {
    return !!roles.filter(x => x.id === role.id)[0];
  }

  //выбираем роль
  selectRole(role: RoleDto, selectedGroup: GroupDto) {

    if (this.isCheckedSelectedRoles(this.selectedRoles, role)) {
      // если роль на которую кликнули уже была отмечена
      // значит роль исключили из списка, находим индекс этой роли и удаляем ее из массива
      let selectedRole = this.selectedRoles.filter(x => x.id === role.id)[0]
      const index = this.selectedRoles.indexOf(selectedRole);
      if (index > -1) {
        this.selectedRoles.splice(index, 1);

        //при смене страницы сохраняем или удаляем роль у выбранной группы
        if (selectedGroup.roles) {
          let roleInSelectedGroup = selectedGroup.roles.filter(x => x.id === role.id)[0]
          const roleInSelectedGroupIndex = selectedGroup.roles.indexOf(roleInSelectedGroup);
          selectedGroup.roles?.splice(roleInSelectedGroupIndex, 1);
        }
      }
    } else {
      // если роль на которую кликнули еще не была отмечена
      // значит роль добавили в список, добавляем роль в список выбранных ролей и в список ролей выбранной группы на случай смены пагинации
      this.selectedRoles.push(role)
      selectedGroup.roles?.push(role)
    }
  }

  //создать группу
  createGroup(data: GroupDto): void {
    this.isLoading = true
    this.clientApi.createGroup(data)
      .subscribe({
        next: () => {
          this.isLoading = false
          //после создания группы обновляем список ролей
          this.groupsSbj.next(null)
        }
      })

    //перезагрузка модального окна
    //изза запрета использовать css фреймворки у меня возникли пробелы с обработкой модального окна на чистом css
    //поэтому пришлось прибегнуть к такому решению
    this.reloadModalComponent()
  }

  //удалить группу
  deleteGroup(groupId: number): void {
    this.isLoading = true
    this.clientApi.deleteGroup(groupId)
      .subscribe({
        next: () => {
          this.isLoading = false
          //после удаления группы обновляем список ролей
          this.groupsSbj.next(null)
        }
      })
  }

  //редактирование группы
  submit() {
    this.isLoading = true
    if (this.dataForm?.invalid) {
      this.errorSbj.next("Введите корректные данные")
      return;
    }

    let group = this.dataForm?.value as GroupDto
    group.roles = this.selectedRoles // заменяем список ролей роли на список выбранных на панели ролей
    this.clientApi.editGroup(group)
      .subscribe({
        next: () => {
          this.isLoading = false
          this.groupsSbj.next(null)
        }
      })
  }

  //поиск
  onSearch(search: string) {
    let asd = this.filteredRoles.filter(role => search && role.name?.includes(search))

    if (asd.length > 0) {
      this.filteredRoles = asd
    } else {
      this.filteredRoles = this.allRoles
    }
  }

  //установить фильтр
  setFilter(filter: any) {
    this.filteredRoles = this.allRoles
    switch (filter) {
      case 'all':
        this.filteredRoles = this.allRoles
        break
      case 'isNotSelected':
        this.filteredRoles = this.filteredRoles.filter(role => role.isSelected === false)
        break
      case 'isSelected':
        this.filteredRoles = this.filteredRoles.filter(role => role.isSelected === true)
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

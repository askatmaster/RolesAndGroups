import {ErrorHandler, Inject, Injectable, Injector, NgZone} from "@angular/core";
import {ErrorResponse} from "../api/client-api";

@Injectable()
export class GlobalErrorHandlerService extends ErrorHandler {
  private elaborating: boolean = false;
  injector: Injector;
  ngzone: NgZone;
  constructor() {
    super();
    this.injector = Inject(Injector)
    this.ngzone = Inject(NgZone)
  }

  //Глобавльный обработчик ошибок (пока что он просто записывает в консоль, но можно расширить функционал)
  override handleError(error: ErrorResponse): void {
    if (!this.elaborating) {
      this.elaborating = true;

      this.ngzone.run(() => {
        console.log(error.errorMessage)
      })
    }
    super.handleError(error);
  }
}

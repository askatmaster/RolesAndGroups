import {Injectable, OnDestroy} from '@angular/core';
import {Observable, Subject} from 'rxjs';

@Injectable()
export abstract class DestroyComponent implements OnDestroy {

  private destroyedSbj = new Subject<boolean>();

  protected get destroyed$(): Observable<boolean> {
    return this.destroyedSbj;
  }

  ngOnDestroy(): void {
    this.destroyedSbj.next(true);
    this.destroyedSbj.complete();
  }
}

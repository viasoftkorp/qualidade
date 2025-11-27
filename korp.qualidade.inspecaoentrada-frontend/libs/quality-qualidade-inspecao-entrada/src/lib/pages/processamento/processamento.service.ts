import { Injectable } from '@angular/core';

import { Subject } from 'rxjs';

@Injectable()
export class ProcessamentoInspecaoService {
  public actionsTemplate: Subject<any> = new Subject();
}

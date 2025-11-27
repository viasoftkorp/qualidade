import { Injectable } from '@angular/core';

import { Subject } from 'rxjs';

@Injectable()
export class HistoricoInspecaoService {
  public actionsTemplate: Subject<any> = new Subject();
}

import {
  Component,
  OnDestroy
} from '@angular/core';
import { VsSubscriptionManager } from '@viasoft/common';

@Component({
  selector: 'qa-processamento-inspecao',
  templateUrl: './processamento-inspecao.component.html',
  styleUrls: ['./processamento-inspecao.component.scss']
})
export class ProcessamentoInspecaoComponent implements OnDestroy {
  private subs = new VsSubscriptionManager();

  constructor() { }

  ngOnDestroy(): void {
    this.subs.clear();
  }
}

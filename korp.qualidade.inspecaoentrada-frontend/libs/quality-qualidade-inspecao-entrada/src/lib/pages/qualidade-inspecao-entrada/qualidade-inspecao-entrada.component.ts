import {
  Component,
  OnDestroy
} from '@angular/core';
import { VsSubscriptionManager } from '@viasoft/common';
import {VsTabDirective} from "@viasoft/components";

@Component({
  selector: 'qa-qualidade-inspecao-entrada',
  templateUrl: './qualidade-inspecao-entrada.component.html',
  styleUrls: ['./qualidade-inspecao-entrada.component.scss']
})
export class QualidadeInspecaoEntradaComponent implements OnDestroy {
  public tab: string;
  private subs = new VsSubscriptionManager();

  public ngOnDestroy(): void {
    this.subs.clear();
  }

  public onTabChanged(tab: VsTabDirective): void {
    this.tab = tab.vsTabLabel.title
  }
}

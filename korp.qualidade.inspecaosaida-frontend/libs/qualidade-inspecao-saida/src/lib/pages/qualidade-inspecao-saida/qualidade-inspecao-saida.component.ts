import { Component, OnDestroy } from '@angular/core';
import { VsSubscriptionManager } from '@viasoft/common';
import { VsTabDirective } from '@viasoft/components';

@Component({
  selector: 'qa-qualidade-inspecao-saida',
  templateUrl: './qualidade-inspecao-saida.component.html',
  styleUrls: ['./qualidade-inspecao-saida.component.scss']
})
export class QualidadeInspecaoSaidaComponent implements OnDestroy {
  public tab: string;
  private subs = new VsSubscriptionManager();

  ngOnDestroy(): void {
    this.subs.clear();
  }

  public onTabChanged(tab: VsTabDirective): void {
    this.tab = tab.vsTabLabel.title
  }
}

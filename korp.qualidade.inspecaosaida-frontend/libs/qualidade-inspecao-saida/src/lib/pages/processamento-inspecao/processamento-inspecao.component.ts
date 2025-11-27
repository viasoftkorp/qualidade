import { ChangeDetectorRef, Component, OnDestroy, TemplateRef, ViewChild } from '@angular/core';

import { VsSubscriptionManager } from '@viasoft/common';
import { TabsViewTemplateComponent } from '@viasoft/view-template';

import { ProcessamentoInspecaoService } from './processamento-inspecao.service';

@Component({
  selector: 'qa-processamento-inspecao',
  templateUrl: './processamento-inspecao.component.html',
  styleUrls: ['./processamento-inspecao.component.scss']
})
export class ProcessamentoInspecaoComponent implements OnDestroy {
  @ViewChild(TabsViewTemplateComponent) private tabsViewTemplateComponent: TabsViewTemplateComponent;

  public actionsTemplate: TemplateRef<any>;

  private subs = new VsSubscriptionManager();

  constructor(private service: ProcessamentoInspecaoService, private cdRef: ChangeDetectorRef) {
    this.setActions();
  }

  ngOnDestroy(): void {
    this.subs.clear();
  }

  private setActions(): void {
    this.subs.add('set-actions', this.service.actionsTemplate.subscribe((actionsTemplate: TemplateRef<any>) => {
      this.actionsTemplate = actionsTemplate;
      this.tabsViewTemplateComponent.hideHeader = false;
      this.cdRef.detectChanges();
    }));
  }
}

import { ChangeDetectorRef, Component, OnDestroy, TemplateRef, ViewChild } from '@angular/core';

import { VsSubscriptionManager } from '@viasoft/common';
import { TabsViewTemplateComponent } from '@viasoft/view-template';

import { QualidadeInspecaoSaidaService } from '../../services/qualidade-inspecao-saida.service';
import {VsTabDirective} from "@viasoft/components";

@Component({
  selector: 'qa-qualidade-inspecao-saida',
  templateUrl: './qualidade-inspecao-saida.component.html',
  styleUrls: ['./qualidade-inspecao-saida.component.scss']
})
export class QualidadeInspecaoSaidaComponent implements OnDestroy {
  @ViewChild(TabsViewTemplateComponent) private tabsViewTemplateComponent: TabsViewTemplateComponent;

  public actionsTemplate: TemplateRef<any>;
  public tab: string;

  private subs = new VsSubscriptionManager();

  constructor(private service: QualidadeInspecaoSaidaService, private cdRef: ChangeDetectorRef) {
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

  public onTabChanged(tab: VsTabDirective): void {
    this.tab = tab.vsTabLabel.title
  }
}

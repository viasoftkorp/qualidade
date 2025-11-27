import {
  ChangeDetectorRef,
  Component,
  OnDestroy,
  TemplateRef,
  ViewChild
} from '@angular/core';
import { VsSubscriptionManager } from '@viasoft/common';
import { TabsViewTemplateComponent } from '@viasoft/view-template';
import { QualidadeInspecaoEntradaService } from '../../services/qualidade-inspecao-entrada.service';
import {VsTabDirective} from "@viasoft/components";

@Component({
  selector: 'qa-qualidade-inspecao-entrada',
  templateUrl: './qualidade-inspecao-entrada.component.html',
  styleUrls: ['./qualidade-inspecao-entrada.component.scss']
})
export class QualidadeInspecaoEntradaComponent implements OnDestroy {
  @ViewChild(TabsViewTemplateComponent) private tabsViewTemplateComponent: TabsViewTemplateComponent;

  public actionsTemplate: TemplateRef<any>;
  public tab: string;

  private subs = new VsSubscriptionManager();

  constructor(private service: QualidadeInspecaoEntradaService, private cdRef: ChangeDetectorRef) {
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

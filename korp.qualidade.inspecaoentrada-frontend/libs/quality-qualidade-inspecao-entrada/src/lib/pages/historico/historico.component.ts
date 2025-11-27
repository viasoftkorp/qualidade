import {
  ChangeDetectorRef,
  Component,
  OnDestroy,
  TemplateRef,
  ViewChild
} from '@angular/core';
import { VsSubscriptionManager } from '@viasoft/common';
import { TabsViewTemplateComponent } from '@viasoft/view-template';
import { HistoricoService } from '../../services/historico.service';

@Component({
  selector: 'inspecao-entrada-historico',
  templateUrl: './historico.component.html',
  styleUrls: ['./historico.component.scss']
})
export class HistoricoComponent implements OnDestroy {
  @ViewChild(TabsViewTemplateComponent) private tabsViewTemplateComponent: TabsViewTemplateComponent;

  public actionsTemplate: TemplateRef<any>;

  private subs = new VsSubscriptionManager();

  constructor(private service: HistoricoService, private cdRef: ChangeDetectorRef) {
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

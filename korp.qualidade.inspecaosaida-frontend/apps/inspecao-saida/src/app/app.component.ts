import { Component } from '@angular/core';
import { VsAuthService } from '@viasoft/common';
import { IS_ON_PREMISE } from '../environments/is-on-premise.const';

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  styleUrls: ['app.component.scss']
})
export class AppComponent {
  public isOnPremise = IS_ON_PREMISE;

  constructor(private auth: VsAuthService) { }

  public requestSignOut(): void {
    this.auth.logout();
  }
}

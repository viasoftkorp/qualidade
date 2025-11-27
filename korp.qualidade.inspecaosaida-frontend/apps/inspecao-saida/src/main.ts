import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { initAppSettings } from '@viasoft/common';
import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

initAppSettings(environment).finally(() => {
  platformBrowserDynamic().bootstrapModule(AppModule)
    .catch((err) => console.error(err));
});

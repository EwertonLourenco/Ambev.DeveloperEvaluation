import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { provideHttpClient, withInterceptors } from '@angular/common/http';

import { AppComponent } from './app/app.component';
import { routes } from './app/routes';
import { BaseUrlInterceptor } from './app/core/http/base-url.interceptor';
import { AuthInterceptor } from './app/core/http/auth.interceptor';

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptors([BaseUrlInterceptor, AuthInterceptor])),
  ],
}).catch((err: unknown) => console.error(err));

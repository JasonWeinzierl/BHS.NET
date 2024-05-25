import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { ErrorHandler, NgModule, Optional, SkipSelf } from '@angular/core';
import { BrowserModule, Title } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { AuthHttpInterceptor, AuthModule } from '@auth0/auth0-angular';
import { ApplicationinsightsAngularpluginErrorService } from '@microsoft/applicationinsights-angularplugin-js';
import { AlertModule } from 'ngx-bootstrap/alert';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { MarkdownModule } from 'ngx-markdown';
import { ToastrModule } from 'ngx-toastr';
import { ContentLayoutComponent } from './components/content-layout/content-layout.component';
import { FooterComponent } from './components/footer/footer.component';
import { HeaderComponent } from './components/header/header.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { throwIfAlreadyLoaded } from './guards/module-import.guard';
import { auth0ConfigProvider } from './providers/auth0-config.provider';
import { bootstrapMarkedOptionsProvider } from './providers/bootstrap-marked-options.provider';

/**
 * Resources used by AppModule which are always and only loaded once.
 *
 * Examples:
 *  - Route guards
 *  - HTTP interceptors
 *  - App-level services and components
 *  - Logging
 */
@NgModule({
  imports: [
    NotFoundComponent,
    ContentLayoutComponent,
    HeaderComponent,
    FooterComponent,

    // angular
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    RouterModule,

    // auth0
    AuthModule.forRoot(),

    // ngx-markdown
    MarkdownModule.forRoot({
      markedOptions: [bootstrapMarkedOptionsProvider],
    }),

    // ngx-bootstrap
    AlertModule.forRoot(),
    BsDatepickerModule.forRoot(),
    BsDropdownModule.forRoot(),
    CarouselModule.forRoot(),
    CollapseModule.forRoot(),

    // ngx-toastr
    ToastrModule.forRoot(),
  ],
  providers: [
    Title,
    {
      provide: ErrorHandler,
      useClass: ApplicationinsightsAngularpluginErrorService,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthHttpInterceptor,
      multi: true,
    },
    auth0ConfigProvider,
  ],
})
export class CoreModule {
  // TODO: once we convert to Standalone components, this is the last decorator preventing us from disabling experimentalDecorators and turning on the lint rule `consistent-type-imports`.
  constructor(@Optional() @SkipSelf() parentModule: CoreModule | null) {
    throwIfAlreadyLoaded(parentModule, 'CoreModule');
  }
}

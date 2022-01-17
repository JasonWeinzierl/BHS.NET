import { BrowserModule, Title } from '@angular/platform-browser';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { bootstrapMarkedOptionsProvider } from '@core/provider/bootstrap-marked-options.provider';
import { ContentLayoutComponent } from './layout/content-layout/content-layout.component';
import { CoreModule } from '@core/core.module';
import { FooterComponent } from './layout/footer/footer.component';
import { HeaderComponent } from './layout/header/header.component';
import { HttpClientModule } from '@angular/common/http';
import { MarkdownModule } from 'ngx-markdown';
import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    ContentLayoutComponent,
  ],
  imports: [
    // angular
    BrowserModule,
    HttpClientModule,

    // 3rd party
    MarkdownModule.forRoot({
      markedOptions: [bootstrapMarkedOptionsProvider]
    }),

    // core & shared
    CoreModule,
    SharedModule,

    // app
    AppRoutingModule
  ],
  providers: [Title],
  bootstrap: [AppComponent]
})
export class AppModule { }

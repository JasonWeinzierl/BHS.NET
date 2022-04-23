import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { CoreModule } from '@core/core.module';
import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    CoreModule, // singletons, forRoot imports, app-level components
    SharedModule, // modules, components, directives, etc. used by more than one module

    AppRoutingModule, // app-level routing (with lazy-loading)
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

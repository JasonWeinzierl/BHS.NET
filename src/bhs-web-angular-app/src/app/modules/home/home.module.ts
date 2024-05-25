import { NgModule } from '@angular/core';
import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './pages/home.component';
import { SharedModule } from '@shared/shared.module';

@NgModule({
  imports: [
    HomeComponent,

    SharedModule,
    HomeRoutingModule,
  ],
})
export class HomeModule { }

import { LocationComponent } from './page/location.component';
import { LocationRoutingModule } from './location-routing.module';
import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';

@NgModule({
  declarations: [LocationComponent],
  imports: [
    LocationRoutingModule,
    SharedModule,
  ]
})
export class LocationModule { }

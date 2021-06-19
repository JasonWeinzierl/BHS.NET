import { NgModule } from '@angular/core';

import { SharedModule } from '@shared/shared.module';

import { LocationRoutingModule } from './location-routing.module';
import { LocationComponent } from './page/location.component';

@NgModule({
  declarations: [LocationComponent],
  imports: [
    LocationRoutingModule,
    SharedModule,
  ]
})
export class LocationModule { }

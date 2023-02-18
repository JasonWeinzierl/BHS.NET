import { LocationComponent } from './pages/location.component';
import { LocationRoutingModule } from './location-routing.module';
import { NgModule } from '@angular/core';
import { SharedModule } from '@shared/shared.module';

@NgModule({
  declarations: [LocationComponent],
  imports: [
    SharedModule,
    LocationRoutingModule,
  ],
})
export class LocationModule { }

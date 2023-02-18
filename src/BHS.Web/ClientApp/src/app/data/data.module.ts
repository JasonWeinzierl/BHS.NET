import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';

/**
 * Modules and services for data consumed by this app.
 *
 * Do not create data modules for each feature.
 */
@NgModule({
  declarations: [],
  imports: [
    HttpClientModule,
  ],
})
export class DataModule { }

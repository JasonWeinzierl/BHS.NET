import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AlertModule } from 'ngx-bootstrap/alert';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { SortByPipe } from './pipes/sort-by.pipe';

/**
 * Resources used by more than one module.
 *
 * Includes exports of widely-used Angular-built modules (e.g. CommonModule) or 3rd-party modules (e.g. bootstrap).
 * Laxy-loading feature modules which import SharedModule can use these modules without separately importing them.
 */
@NgModule({
  imports: [
    CommonModule,
  ],
  declarations: [SortByPipe],
  exports: [
    SortByPipe,

    // Make these available for all modules.
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    AlertModule, // ngx-bootstrap
    BsDropdownModule, // ngx-bootstrap
    CarouselModule, // ngx-bootstrap
    CollapseModule, // ngx-bootstrap
  ],
})
export class SharedModule { }

import { CommonModule, NgOptimizedImage } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AlertModule } from 'ngx-bootstrap/alert';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ToastrModule } from 'ngx-toastr';
import { DateComponent } from './components/date/date.component';
import { SnippetPipe } from './pipes/snippet.pipe';
import { SortByPipe } from './pipes/sort-by.pipe';

/**
 * Resources used by more than one module.
 *
 * Includes exports of widely-used Angular-built modules (e.g. CommonModule) or 3rd-party modules (e.g. bootstrap).
 * Lazy-loading feature modules which import SharedModule can use these modules without separately importing them.
 */
@NgModule({
  imports: [
    CommonModule,
    NgOptimizedImage,
  ],
  declarations: [
    SortByPipe,
    SnippetPipe,
    DateComponent,
  ],
  exports: [
    SortByPipe,
    SnippetPipe,
    DateComponent,

    // Make these available for all modules.
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgOptimizedImage,
    AlertModule, // ngx-bootstrap
    BsDatepickerModule, // ngx-bootstrap
    BsDropdownModule, // ngx-bootstrap
    CarouselModule, // ngx-bootstrap
    CollapseModule, // ngx-bootstrap
    ToastrModule, // ngx-toastr
  ],
})
export class SharedModule { }

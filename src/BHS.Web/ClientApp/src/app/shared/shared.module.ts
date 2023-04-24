import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AlertModule } from 'ngx-bootstrap/alert';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { CommonModule } from '@angular/common';
import { DateComponent } from './components/date/date.component';
import { NgModule } from '@angular/core';
import { SnippetPipe } from './pipes/snippet.pipe';
import { SortByPipe } from './pipes/sort-by.pipe';
import { ToastrModule } from 'ngx-toastr';

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
    AlertModule, // ngx-bootstrap
    BsDatepickerModule, // ngx-bootstrap
    BsDropdownModule, // ngx-bootstrap
    CarouselModule, // ngx-bootstrap
    CollapseModule, // ngx-bootstrap
    ToastrModule, // ngx-toastr
  ],
})
export class SharedModule { }

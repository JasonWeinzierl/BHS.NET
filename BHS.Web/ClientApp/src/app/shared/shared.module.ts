import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SortByPipe } from './pipe/sort-by.pipe';

@NgModule({
  imports: [
    CommonModule,
  ],
  declarations: [SortByPipe],
  exports: [SortByPipe],
})
export class SharedModule { }

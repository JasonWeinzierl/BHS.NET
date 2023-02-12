/* eslint-disable @angular-eslint/directive-class-suffix */
/* eslint-disable @angular-eslint/directive-selector */
import { Directive, Input } from '@angular/core';
import { IsActiveMatchOptions } from '@angular/router';

@Directive({
  selector: '[routerLinkActive]',
})
export class RouterLinkActiveDirectiveStub {
  @Input() routerLinkActive: string[] | string = '';
  @Input() routerLinkActiveOptions: { exact: boolean } | IsActiveMatchOptions = { exact: false };
}

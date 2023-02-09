// This file is required by karma.conf.js and loads recursively all the .spec and framework files

import {
  BrowserDynamicTestingModule,
  platformBrowserDynamicTesting
} from '@angular/platform-browser-dynamic/testing';
import { getTestBed } from '@angular/core/testing';

// First, initialize the Angular testing environment.
getTestBed().initTestEnvironment(
  BrowserDynamicTestingModule,
  platformBrowserDynamicTesting(),
  {
    // Starting in Angular 15, these default to true.
    // TODO: Fix unit tests broken by this change.
    // Once fixed, you can remove this file and all references to it.
    errorOnUnknownElements: false,
    errorOnUnknownProperties: false,
  },
);

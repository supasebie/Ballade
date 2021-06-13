import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { HttpClientModule } from '@angular/common/http';

import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { NgModule } from '@angular/core';

const commonModules = [
  BrowserAnimationsModule,
  HttpClientModule,
  BrowserModule,
  CommonModule,
  FormsModule
];

@NgModule({
  declarations: [],
  imports: [...commonModules],
  exports: [...commonModules]
})
export class AngularCommonModule {}

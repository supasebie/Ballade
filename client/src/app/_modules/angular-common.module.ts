import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

const commonModules = [
  BrowserAnimationsModule,
  HttpClientModule,
  BrowserModule,
  CommonModule,
  FormsModule,
  ReactiveFormsModule
];

@NgModule({
  declarations: [],
  imports: [...commonModules],
  exports: [...commonModules]
})
export class AngularCommonModule {}

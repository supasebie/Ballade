import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { NgxSpinnerModule } from 'ngx-spinner';
import { FlexLayoutModule } from '@angular/flex-layout';

import { NgModule } from '@angular/core';

@NgModule({
  declarations: [],
  imports: [FlexLayoutModule, NgxGalleryModule, NgxSpinnerModule],
  exports: [FlexLayoutModule, NgxGalleryModule, NgxSpinnerModule]
})
export class NpmModule {}

import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { FlexLayoutModule } from '@angular/flex-layout';

import { NgModule } from '@angular/core';

@NgModule({
  declarations: [],
  imports: [FlexLayoutModule, NgxGalleryModule],
  exports: [FlexLayoutModule, NgxGalleryModule]
})
export class NpmModule {}

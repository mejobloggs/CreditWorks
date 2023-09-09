import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { TableSortableComponent } from './table-sortable/table-sortable.component';

@NgModule({
  declarations: [
    AppComponent,
    TableSortableComponent,
  ],
  imports: [
    BrowserModule, HttpClientModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

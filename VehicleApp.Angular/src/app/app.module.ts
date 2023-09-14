import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { TableSortableComponent } from './table-sortable/table-sortable.component';
import { VehicleEditComponent } from './vehicle-edit/vehicle-edit.component';
import { VehicleListComponent } from './vehicle-list/vehicle-list.component';
import { CategoriesEditComponent } from './categories-edit/categories-edit.component';

@NgModule({
  declarations: [
    AppComponent,
    TableSortableComponent,
    VehicleEditComponent,
    VehicleListComponent,
    CategoriesEditComponent,
  ],
  imports: [
    BrowserModule, HttpClientModule, FormsModule,
    RouterModule.forRoot([
      { path: '', redirectTo: '/vehicles', pathMatch: 'full' },
      { path: 'vehicles', component: VehicleListComponent },
      { path: 'vehicles/add', component: VehicleEditComponent },
      { path: 'vehicles/edit/:id', component: VehicleEditComponent },
      { path: 'categories', component: CategoriesEditComponent },
    ]),
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

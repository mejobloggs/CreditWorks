import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { TableDataFormat, TableColumn, DataTypeEnum } from './table-sortable/table-sortable.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  public vehicles?: VehicleView[];
  public vehiclesTableData: TableDataFormat = { columns: [], data: [] };

  constructor(http: HttpClient) {
    http.get<VehicleView[]>('/vehicles').subscribe(result => {
      this.vehicles = result;

      this.vehiclesTableData = { columns: this.generateVehicleColumns(), data: this.vehicles };


    }, error => console.error(error));
  }

  title = 'VehicleApp.Angular';

  private generateVehicleColumns(): TableColumn[] {
    return [
      { title: 'Owner', field: 'ownerName' },
      { title: 'Manufacturer', field: 'manufacturer' },
      { title: 'Year', field: 'manufactureYear'},
      { field: 'weightInKg', hidden: true },
      { title: 'Category', field: 'categoryImage', sortOnField: 'weightInKg', datatype: DataTypeEnum.Image},
    ];
  }

}

interface VehicleView {
  ownerName: string;
  manufactureYear: number;
  weightInKg: number;
  manufacturer: string;
  category: string;
  categoryImage: string;
}

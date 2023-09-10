import { Component } from '@angular/core';

import { TableDataFormat, TableColumn, DataTypeEnum } from '../table-sortable/table-sortable.model';
import { VehicleView } from '../shared.models';
import { ApiService } from '../api.service';

@Component({
  selector: 'app-vehicle-list',
  templateUrl: './vehicle-list.component.html',
  styleUrls: ['./vehicle-list.component.css']
})
export class VehicleListComponent {

  public vehicles?: VehicleView[];
  public vehiclesTableData: TableDataFormat = { columns: [], data: [] };
  constructor(private apiService: ApiService) { }

  ngOnInit(): void {
    this.apiService.getVehicles().subscribe(result => {
      this.vehicles = result;
      this.vehiclesTableData = { columns: this.generateVehicleColumns(), data: this.vehicles, editRoute: "vehicles/edit" };
    }, error => console.error(error))
  }

  private generateVehicleColumns(): TableColumn[] {
    return [
      { title: 'Owner', field: 'ownerName' },
      { title: 'Manufacturer', field: 'manufacturer' },
      { title: 'Year', field: 'manufactureYear' },
      { field: 'weightInKg', hidden: true },
      { title: 'Category', field: 'categoryImage', sortOnField: 'weightInKg', datatype: DataTypeEnum.Image },
    ];
  }
}

import { Component, Input } from '@angular/core';
import { TableDataFormat, DataTypeEnum } from './table-sortable.model';
import { Router } from '@angular/router';


@Component({
  selector: 'app-table-sortable',
  templateUrl: './table-sortable.component.html',
  styleUrls: ['./table-sortable.component.scss']
})
export class TableSortableComponent {
  @Input() tableData: TableDataFormat = { columns: [], data: [] };
  public dataTypeEnum = DataTypeEnum;
  private sortingDirections: { [field: string]: 'asc' | 'desc' } = {};

  constructor(private router: Router) { }

  navigateToEdit(entityId: number): void {
    if (this.tableData.editRoute) {
      this.router.navigate([this.tableData.editRoute, entityId]);
    }
  }

  sortItems(field: string): void {
    const currentDirection = this.sortingDirections[field] || 'asc';
    const newDirection = currentDirection === 'asc' ? 'desc' : 'asc';
    this.sortingDirections[field] = newDirection;


    this.tableData.data.sort((a, b) => {
      if (a[field] < b[field]) {
        return newDirection === 'asc' ? -1 : 1;
      }
      if (a[field] > b[field]) {
        return newDirection === 'asc' ? 1 : -1;
      }
      return 0;
    });
  }

}

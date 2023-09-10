import { Component } from '@angular/core';
import { ApiService } from '../api.service';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-categories-edit',
  templateUrl: './categories-edit.component.html',
  styleUrls: ['./categories-edit.component.scss']
})
export class CategoriesEditComponent {
  categories: Category[] = [];
  errorMessage?: string;

  constructor(
    private apiService: ApiService) { }

  ngOnInit(): void {
    this.apiService.getCategories().subscribe(c => { this.categories = c });
  }

  save(): void {
    this.apiService.saveCategories(this.categories).subscribe((c) => { this.categories = c },
      (error) => {
        this.errorMessage = "Save failed: " + error.error
      });
  }

  addNew(): void {
    this.categories.push({});
  }

  remove(category: Category): void {
    category.delete = true;
  }
}

export interface Category {
  id?: number;
  name?: string;
  minKg?: number;
  maxKg?: number;
  imageName?: string;
  rowVersion?: Uint8Array,
  delete?: boolean
}

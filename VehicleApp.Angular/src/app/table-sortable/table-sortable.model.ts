export interface TableColumn {
  title?: string;
  field: string;
  sortOnField?: string;
  hidden?: boolean;
  datatype?: DataTypeEnum;
}

export interface TableDataFormat {
  columns: TableColumn[];
  data: any[];
}


export enum DataTypeEnum {
  Unspecified,
  Image
}


export interface Directory {
  id?: string,
  name?: string,
  description?: string,
  directoryDomainId?: string,
  directoryLevelId?: string,
  directoryDomain?: DirectoryDomain,
  directoryLevel?: DirectoryLevel
}

export interface DirectoryVersion {
  id?: string,
  version?: string,
  versionDate?: string,
  versionDescription?: string,
  dataSourceDate?: string,
  dataSourceName?: string,
  dataSourceUrl?: string,
  tableName?: string,
  directoryId?: string | string[],
  directory?: Directory
}

export interface TableRow {
  id: number,
  name: string,
  description: string,
  field1: string
}

export interface DirectoryLevel {
  id?: string,
  name?: string
}

export interface DirectoryDomain {
  id?: string,
  name?: string
}

export type MdmTablesData = {
  tableName: string;
  mainTable: Array<any>;
  foreignTables?: any;
}

export type MdmTableData = {
  tablename: string;
  object?: MdmObject;
  objects?: Array<MdmObject>;
  ids?: Array<number | string>;
}

export type MdmObject = {
  id: number;
  properties: Array<Property>;
}

export type Property = {
  propName: string;
  propValue: string | number;
  objectFk: MdmObject;
}

export type TableColumnsRequest = {
  tablename: string;
  columnName: string;
}

export type DeleteDataRequest = {
  tablename: string;
  ids: Array<string>;
}

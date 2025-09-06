export interface InventarioItem {
  id: number;
  idInventario: number;
  referenciaPeso: string;
  pesoActual: number;
  fechaRegistroItem: Date;
  estado: string;
}
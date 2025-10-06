export interface Cliente {
  id?: number;      // <-- obligatorio para PUT
  nombre: string;
  direccion?: string;
  telefono?: string;
  email?: string;
}
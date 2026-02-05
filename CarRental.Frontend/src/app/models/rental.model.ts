export interface Rental {
  id: number;
  userId: number;
  carId: number;
  startDate: Date;
  endDate: Date;
  price: number;
}

export interface RentalDto {
  userId: number;
  carId: number;
  startDate: Date;
  endDate: Date;
}

export interface RentalResult {
  success: boolean;
  rental?: Rental;
  totalPrice?: number;
  error?: string;
}

import { User } from './user';

export const OrderType = {
  LastActive: 'lastActive',
  Name: 'name',
  Age: 'age'
} as const;

export class UserParams {
  gender: string;
  minAge = 19;
  maxAge = 99;
  pageNumber = 0;
  pageSize = 5;
  orderBy: string;

  constructor(user: User) {
    this.gender = user.gender === 'female' ? 'male' : 'female';
    this.orderBy = OrderType.LastActive;
  }
}

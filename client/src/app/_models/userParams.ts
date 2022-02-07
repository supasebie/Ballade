import { BaseParams } from './baseParams';
import { User } from './user';

export const OrderType = {
  LastActive: 'lastActive',
  Name: 'name',
  Age: 'age'
} as const;

export class UserParams extends BaseParams {
  gender: string;
  minAge = 0;
  maxAge = 99;
  orderBy: string;
  predicate: string = 'liked';

  constructor(user: User) {
    super();
    this.gender = user.gender === 'female' ? 'male' : 'female';
    this.orderBy = OrderType.LastActive;
  }
}

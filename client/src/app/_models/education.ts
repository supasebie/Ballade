export enum Breed {
  ShibaInu,
  Mut,
  Lab,
  WolfHound
}

export class Dog {
  myDogBreed: string = 'a';
  InputDogBreed: string = 'b';
  SaneDogInput: Breed = Breed.ShibaInu;
}

export class Numbers {
  number: number = 1;
  number1: string = '1';
  number3: string = 'NotInt';
}

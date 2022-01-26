export interface User {
  username: string;
  token: string;
  photoUrl: string;
  knownAs: string;
  gender: string;
}

export interface UserRegister {
  Username: string;
  Password: string;
  Gender: string;
  KnownAs: string;
  City: string;
  Country: string;
  DateOfBirth: Date;
}

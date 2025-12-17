export const UserRole = {
  Unspecified: 0,
  Photographer: 1,
  Client: 2
} as const;

export type UserRole = typeof UserRole[keyof typeof UserRole];

export interface LoginDto {
  username: string;
  password: string;
}

export interface RegisterDto {
  username: string;
  password: string;
  firstName: string;
  lastName: string;
  role: UserRole;
}

export interface AuthResponseDto {
  userId: number;
  username: string;
  firstName: string;
  lastName: string;
  role: UserRole;
  token: string;
}

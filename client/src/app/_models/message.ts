export interface Message {
  id: number;
  senderUsername: string;
  senderId: number;
  senderPhotoUrl: string;
  recipientUsername: string;
  recipientId: number;
  recipientPhotoUrl: string;
  content: string;
  dateSent: Date;
  dateRead?: Date;
  senderDeleted: boolean;
  recipientDeleted: boolean;
}

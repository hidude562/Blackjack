using System;
using System.Collections.Generic;
using System.Linq;

namespace Blackjack {
    // Structure for Dealer's cards
    class Cards {
        // A typical deck of cards has 52 cards
        public int num_cards;
        public int cards_per_type;
        public int card_types;
        public int[] deck_ids;
        public int cards_left;
        public string[] deck_names;
        public string[] id_to_name;
        public int[] id_to_value;


        public void convert_card_ids_to_names() {
            int index = 0;
            foreach (int card in this.deck_ids) {
                this.deck_names[index] = this.id_to_name[card];
                index++;
            }
        }
        public void New_cards() {
            // Create the sorted array of cards
            cards_left = this.num_cards;
            for(int i = 0; i < this.num_cards / this.cards_per_type; i++){
                for(int r = 0; r < this.cards_per_type; r++){
                    this.deck_ids[(i * 4) + r] = i;
                }
            }
        }

        // Pop id, name, and value of a card
        public int pop_card() { 
            int value_id = this.deck_ids[cards_left - 1];
            cards_left--;
            // *TODO* Pop equivilent for C# (return popped value)
            if(this.cards_left == 0) {
                New_cards();
                Shuffle(200);
            }
            return value_id;
        }

        public Cards() {
            this.num_cards = 52;
            this.cards_left = this.num_cards;
            this.cards_per_type = 4;
            this.card_types = 13;
            this.deck_ids = new int[this.num_cards];
            this.deck_names = new string[this.num_cards];
            this.id_to_name = new string[this.card_types];
            this.id_to_value = new int[this.card_types];
            string[] id_to_name = {"Ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King"};
            int[] id_to_value = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 10, 10, 10};

            // TBH i have no clue why but just setting the id_to_main doesn't work and i dont feel like fixing it so here is a sudo-fix
            for(int i = 0; i < this.card_types; i++) {
                this.id_to_name[i] = id_to_name[i];
                this.id_to_value[i] = id_to_value[i];
            }

            // Create the sorted array of cards
            New_cards();
        }

        public void Shuffle(int iterations) {
            //TODO: make byte type?
            Console.WriteLine("Shuffling cards....");
            Random rand = new Random();

            int random_1;
            int random_2;
            int intermediate;

            for(int iter = 0; iter < iterations; iter++) {
                // Random integer 0 - 12 (Random function goes up to N-1)
                random_1 = rand.Next(0, this.num_cards);
                random_2 = rand.Next(0, this.num_cards);

                intermediate = this.deck_ids[random_2];

                // Swap the values
                this.deck_ids[random_2] = this.deck_ids[random_1];
                this.deck_ids[random_1] = intermediate;
            }
        }
    }



    class Program {
        static void Main(string[] args) {
            // *TODO* Make these variables in their own seperate class
            double money = 100;
            int your_card_values;
            int your_card_values2;
            int dealer_card_values;
            int card_id;
            int dealer_card_values2;
            int dealer_cards_highest_value;
            int your_cards_highest_value;
            double bet_amount;
            bool you_have_ace;
            bool dealer_has_ace;
            bool game_finished;
            List<int> your_card_ids = new List<int>();
            List<int> dealer_card_ids = new List<int>();

            Cards game_cards = new Cards();
            game_cards.Shuffle(200);
            game_cards.convert_card_ids_to_names();

            // Some methods here:

            void print_cards(List<int> card_ids) {
                foreach(int card in card_ids) {
                    Console.Write($" {game_cards.id_to_name[card]}");
                }
                Console.Write("\n");
            }

            int sum_cards_value(List<int> card_ids, bool is_ace_counted_as_one) {
                int sum = 0;
                foreach(int card in card_ids) {
                    if(card == 0 && !is_ace_counted_as_one) {
                        sum += 11;
                    }
                    sum += game_cards.id_to_value[card];
                }
                return sum;
            }

            void refresh_cards() {
                foreach(int card in your_card_ids) {
                    if(card == 0) {
                        you_have_ace = true;
                    }
                }
                foreach(int card in dealer_card_ids) {
                    if(card == 0) {
                        dealer_has_ace = true;
                    }
                }
            }

            void print_your_card_values() {
                if(you_have_ace) {
                    Console.WriteLine($"Your cards are now worth {your_card_values} or {your_card_values2}");
                } else {
                    Console.WriteLine($"Your cards are now worth {your_card_values} ");
                }
            }

            void print_dealer_card_values() {
                Console.Write($"their cards are now worth {dealer_card_values}");
                if(dealer_has_ace) {
                    Console.WriteLine($", or {dealer_card_values2}");
                } else{
                    Console.Write("\n");
                }
            }

            while(true) {
                while(true) {
                    Console.WriteLine($"You have: ${money}. How much would you like to bet?");
                    bet_amount = Convert.ToDouble(Console.ReadLine());
                    if(!(money < bet_amount) && bet_amount > 0) {
                        break;
                    } else{
                        Console.WriteLine("Invalid!");
                    }
                }

                // INIT
                dealer_card_values = 0;
                your_card_values = 0;
                your_card_values2 = 0;
                your_card_ids = new List<int>();
                dealer_card_ids = new List<int>();
                you_have_ace = false;
                dealer_has_ace = false;
                game_finished = false;

                // Game part:

                // Get dealer card values
                dealer_card_ids.Add(game_cards.pop_card());
                dealer_card_ids.Add(game_cards.pop_card());

                // Get your card values
                your_card_ids.Add(game_cards.pop_card());
                your_card_ids.Add(game_cards.pop_card());
                refresh_cards();

                Console.WriteLine("$----------------------------------------$");
                Console.WriteLine($"The dealer has a {game_cards.id_to_name[dealer_card_ids[0]]}.");
                Console.Write("You have the following cards:");
                print_cards(your_card_ids);
                
                your_card_values = sum_cards_value(your_card_ids, true);
                your_card_values2 = sum_cards_value(your_card_ids, false);

                print_your_card_values();
                while(true) {
                    Console.WriteLine("Would you like to hit or stand? Type hit or stand");
                    if(String.Equals(Console.ReadLine(), "stand")) {
                        break;
                    }
                    card_id = game_cards.pop_card();
                    your_card_ids.Add(card_id);
                    Console.WriteLine($"You got a(n) {game_cards.id_to_name[card_id]}");
                    Console.Write("You now have the following cards:");
                    print_cards(your_card_ids);
                    your_card_values = sum_cards_value(your_card_ids, true);
                    your_card_values2 = sum_cards_value(your_card_ids, false);
                    refresh_cards();

                    print_your_card_values();

                    if(your_card_values > 21) {
                        Console.WriteLine("-- Busted! --");
                        game_finished = true;
                        money -= bet_amount;
                        break;
                    } else if(your_card_values == 21 || your_card_values2 == 21) {
                        Console.WriteLine("-- Blackjack! --");
                        break;
                    }
                }
                dealer_card_values = sum_cards_value(dealer_card_ids, true);
                dealer_card_values2 = sum_cards_value(dealer_card_ids, false);
                refresh_cards();
                Console.Write($"The dealer revealed their hidden card, it was a(n) {game_cards.id_to_value[dealer_card_ids[1]]}, ");
                print_dealer_card_values();

                if(game_finished == false){
                    if(your_card_values2 > 21) {
                        your_cards_highest_value = your_card_values;
                    } else {
                        your_cards_highest_value = your_card_values2;
                    }

                    while(dealer_card_values < 17 && your_cards_highest_value > dealer_card_values) {
                        card_id = game_cards.pop_card();
                        dealer_card_ids.Add(card_id);
                        dealer_card_values = sum_cards_value(dealer_card_ids, true);
                        dealer_card_values2 = sum_cards_value(dealer_card_ids, false);
                        refresh_cards();

                        Console.Write($"The dealer got a(n) {game_cards.id_to_name[card_id]}, ");
                        print_dealer_card_values();

                        if(dealer_card_values2 > 16 && dealer_card_values2 < 22) {
                            break;
                        }
                    }

                    if(dealer_card_values2 > 21) {
                        dealer_cards_highest_value = dealer_card_values;
                    } else {
                        dealer_cards_highest_value = dealer_card_values2;
                    }

                    if(dealer_cards_highest_value > 21) {
                        Console.WriteLine("-- You Won, the dealer busted! --");
                        money+=bet_amount;
                    } else if(your_cards_highest_value > dealer_cards_highest_value) {
                        Console.WriteLine("-- You Won! --");
                        money+=bet_amount;
                    } else if(dealer_cards_highest_value > your_cards_highest_value) {
                        Console.WriteLine("-- You lose! --");
                        money-=bet_amount;
                    } else {
                        Console.WriteLine("-- Its a push! --");
                    }
                }
            }
        }
     }
}
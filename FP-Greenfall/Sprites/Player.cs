using FP_Greenfall.items;
using FP_Greenfall.Resource;
using FP_Greenfall.Sprites.Enemy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace FP_Greenfall.Sprites
{
    public class Player : Character
    {
        private int playerWidth = 70;
        private int playerHeigth = 70;

        private bool movingRight;
        private bool movingLeft;

        private bool jumpKeyHeld;
        private bool canMove;
        const int speed = 10;

        private bool jump = false;
        private bool doubleJump;
        private bool canJump = true;
        private int jumpForce = 60;

        private bool canDashing;
        private bool dash;
        private int dashForce = 5;
        private int step;

        private bool hasKey;
        public bool collideWithStone = false;
        public bool collideWithTree = false;

        private System.Windows.Forms.Timer dashing;
        private System.Windows.Forms.Timer jumpCooldown;
        private System.Windows.Forms.Timer dashCooldown;

        public bool MovingRight
        {
            get { return movingRight; }
        }
        public bool MovingLeft
        {
            get { return movingLeft; }
        }
        public int Health
        {
            get { return health; }
        }
        public int MaxHealth
        {
            get { return maxHealth; }
        }
        public int Speed
        {
            get { return speed; }
        }

        public Player(Point startPosition)
        {
            health = 10;
            maxHealth = 10;
            damage = 1;
            attackRange = playerWidth - 5;

            using(MemoryStream ms  = new MemoryStream(Resource.Player.Player_Walk))
            {
                characterWalkImg = Image.FromStream(ms);
            }
            using (MemoryStream ms = new MemoryStream(Resource.Player.Player_Attack))
            {
                characterAttackImg = Image.FromStream(ms);
            }

            characterImg = characterWalkImg;
            canMove = true;

            currentFrame = 0;
            currentRow = 0;
            totalFrame = 6;

            characterPictureBox = new PictureBox
            {
                Size = new Size(playerWidth, playerHeigth),
                Location = startPosition,
                Image = characterImg,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };


            UpdateCharacter();
            InitializeCooldown();
            InitializeTimer();
            AttackCooldown();
        }

        // Movement Logic
        public void HandleKeyDown(Keys key, Size boundary)
        {
            if (key == Keys.Right && canMove) movingRight = true;
            if (key == Keys.Left && canMove) movingLeft = true;
            if (key == Keys.Space && !jumpKeyHeld && canMove)
            {
                jumpKeyHeld = true;

                if (canJump)
                {
                    jump = true;
                    doubleJump = true;
                    canJump = false;
                } else if (doubleJump)
                {
                    jump = true;
                    doubleJump = false;
                }
            }
            if (key == Keys.Tab && canMove)
            {
                if(!dash) canDashing = true;
            }
            if (key == Keys.W)
            {
                if (!isAttacking) canAttack = true;
            }
        }
        public void HandleKeyUp(Keys key)
        {
            if (key == Keys.Right) movingRight = false;
            if (key == Keys.Left) movingLeft = false;
            if (key == Keys.Space)
            {
                jump = false;
                jumpKeyHeld = false;
            }
        }
        public void StopWalk(Keys key)
        {
            HandleKeyUp(key);
            currentFrame = 0;
            UpdateCharacter();
        }

        // Player Animation
        public void Animation(Size boundary, List<PictureBox> pictureBoxes, int worldY)
        {
            if (characterPictureBox == null) return;

            // Right movement
            if (movingRight)
            {
                facingLeft = false;
                currentFrame = (currentFrame + 1) % totalFrame;
                UpdateCharacter();
            }

            // Left movement
            if (movingLeft)
            {
                facingLeft = true;
                currentFrame = (currentFrame + 1) % totalFrame;
                UpdateCharacter();
            }


            // Jump & double jump
            if (jump)
            {
                characterPictureBox.Top -= jumpForce;
                jumpForce -= gravity;

                if(jumpForce <= 0)
                {
                    jump = false;
                    jumpForce = 60;
                }

                jumpCooldown.Start();
            } else
            {
                // if the player is not on the ground, then gravity pull it down
                ApplyGravity(pictureBoxes);
                jumpForce = 60;
            }

            //Dashing
            if(!dash && canDashing)
            {
                step = 0;
                dash = true;
                canDashing = false;
                dashing.Start();
            }

            //Attack
            if(!isAttacking && canAttack)
            {
                isAttacking = true;
                canAttack = false;
                canMove = false;
                characterImg = characterAttackImg;
                characterPictureBox.Image = characterImg;
                UpdateCharacter();

                Attacking(pictureBoxes);
            }

            if (!canMove && isAttacking)
            {
                currentFrame = (currentFrame + 1) % totalFrame;
                UpdateCharacter();
            }

            if (worldY > 500)
            {
                this.TakeDamage(5, -1);
            }
        }
       
        //Movement logic
        private void Dashing(object sender, EventArgs e)
        {
            if (step < 10)
            {
                int direction = facingLeft ? -1 : 1;
                characterPictureBox.Left += dashForce * direction;
                step += 1;
            } else
            {
                dashCooldown.Start();
                dashing.Stop();
            }
        }
        protected void Attacking(List<PictureBox> pictureBoxes)
        {
            if (facingLeft)
            {
                attackingBox = new Rectangle(
                    characterPictureBox.Left,
                    characterPictureBox.Top,
                    -attackRange,
                    playerHeigth
                );
            } else
            {
                attackingBox = new Rectangle(
                    characterPictureBox.Right,
                    characterPictureBox.Top,
                    attackRange,
                    playerHeigth
                );
            }

            foreach (PictureBox pictureBox in pictureBoxes)
            {
                if (pictureBox != null && pictureBox.Tag is Enemy.Enemy enemy)
                {
                    if (attackingBox.IntersectsWith(pictureBox.Bounds))
                    {
                        enemy.TakeDamage(damage, facingLeft ? -1 : 1);
                    }
                }
            }

            attackTimer.Start();
        }

        //Item Collision
        public void CheckItemCollision(List<Items> items)
        {
            if (characterPictureBox == null || items.Count == 0) return;

            for(int i = items.Count - 1; i >= 0; i--)
            {
                var item = items[i];
                var itembox = item.GetItemPictureBox();

                if(itembox != null && itembox.Tag != null && itembox.Bounds.IntersectsWith(characterPictureBox.Bounds))
                {
                    string tag = itembox.Tag.ToString();

                    switch (tag)
                    {
                        case "HealingItem":
                            AddHealth();
                            break;
                        case "KeyItem":
                            hasKey = true;
                            break;
                        case "Stone":
                            if(hasKey)
                            {
                                collideWithStone = true;
                            } else
                            {
                                continue;
                            }
                            break;
                        case "Tree":
                            collideWithTree = true;
                            break;
                        default: continue;
                    }

                    if(itembox.Parent != null)
                    {
                        itembox.Parent.Controls.Remove(itembox);
                    }

                    itembox.Dispose();
                    items.Remove(item);
                }
            }
        }

        // Healing logic
        private void AddHealth()
        {
            if (health < maxHealth)
            {
                health += 3;
                if(health > maxHealth)
                {
                    health -= health % maxHealth;
                }
                UpdateHealthBar();
            }
        }
        public void UpdateHealthBar()
        {
            Panel healthBar = characterPictureBox?.Parent?.Controls.Find("healthBar", true).FirstOrDefault() as Panel;

            if (healthBar != null)
            {
                int currentHealth = Math.Max(0, this.GetHealth());

                healthBar.Width = (int)(200 * ((double)currentHealth / maxHealth));
            }
        }

        public override void TakeDamage(int damage, int direction)
        {
            if (IsDead() || characterPictureBox == null) return;

            health -= damage;
            UpdateHealthBar();

            knockedBackStep = 0;
            knockBackDirection = direction;
            characterPictureBox.BackColor = Color.Red;
            knockBack.Start();

            if (IsDead())
            {
                GameOver();
            }
        }

        // Cooldown Timer
        private void InitializeCooldown()
        {
            jumpCooldown = new System.Windows.Forms.Timer();
            jumpCooldown.Interval = 2500;
            jumpCooldown.Tick += JumpCooldown;

            dashing = new System.Windows.Forms.Timer();
            dashing.Interval = 15;
            dashing.Tick += Dashing;

            dashCooldown = new System.Windows.Forms.Timer();
            dashCooldown.Interval = 3000;
            dashCooldown.Tick += DashCooldown;
        }
        private void JumpCooldown(object sender, EventArgs e)
        {
            canJump = true;
            jumpCooldown.Stop();
        }
        private void DashCooldown(object sender, EventArgs e)
        {
            dash = false;
            dashCooldown.Stop();
        }
        protected override void AttackCooldown()
        {
            base.AttackCooldown();

            attackTimer.Interval = 240;
            attackTimer.Tick += (s, e) =>
            {
                if (characterPictureBox == null) return;

                characterImg = characterWalkImg;
                characterPictureBox.Image = characterImg;
                UpdateCharacter();
                canMove = true;
                attackTimer.Stop();

                attackCooldown.Start();
            };

            attackCooldown.Interval = 1000;
            attackCooldown.Tick += (s, e) =>
            {
                isAttacking = false;
                attackCooldown.Stop();
            };
        }

        public PictureBox GetPlayerPictureBox() => characterPictureBox;
        public int GetHealth() => health;
        public bool IsMoving() => movingLeft || movingRight || jump;

        private void GameOver()
        {
            dashing?.Stop();
            dashCooldown?.Stop();
            jumpCooldown?.Stop();
            knockBack?.Stop();
            attackCooldown?.Stop();

            characterPictureBox?.Parent?.Controls.Remove(characterPictureBox);
            characterPictureBox?.Dispose();
            characterPictureBox = null;

            MessageBox.Show("Game Over!", "You Died", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Application.Restart();
        }
    }
}

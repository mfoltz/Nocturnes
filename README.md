## Table of Contents

Change config values to suit your server economy.

- [Features](#features)
- [Commands](#commands)
- [Configuration](#configuration)

## Features

- **Sanguis:** Reward players with Sanguis for being online. Can be redeemed in-game for a configurable item reward, Sanguis per minute online and Sanguis per item reward can be configured as well as the update interval.
- **Daily Login Rewards:** Reward players with a congfigurable item/quantity for logging in once per day. Can be separate from Sanguis item.

## Commands

### Sanguis Commands
- `.redeemSanguis`
  - Redeem Sanguis for items.
  - Shortcut: *.rs*
- `.getSanguis`
  - Get total Sanguis. Also updates them.
  - Shortcut: *.get s*
- `.getDaily`  - Get daily time until daily or grants reward if eligible.
  - Shortcut: *.get d*
 
## Configuration

### Reward Systems
- **Sanguis**: `Sanguis` (bool, default: false)  
  Enable or disable Sanguis.
- **Daily Logins**: `DailyLogin` (bool, default: false)  
  Enable or disable daily logins. Sanguis must be enabled as well.
- **Daily Item Reward**: `DailyReward` (int, default: -257494203)  
  Item prefab for daily reward.
- **Daily Item Quantity**: `DailyQuantity` (int, default: 50)
  Item quantity for daily reward.
- **Sanguis Item Reward**: `SanguisItemReward` (int, default: -257494203)  
  Item prefab for redeeming Sanguis.
- **Sanguis Reward Factor**: `SanguisRewardFactor` (int, default: 50)  
  Sanguis required per item reward.
- **Sanguis Per Minute**: `SanguisPerMinute` (int, default: 5)  
  Factor by which rates are increased in expertise/legacy per increment of prestige in leveling.
- **Sanguis Update Interval**: `SanguisUpdateInterval` (int, default: 30)  
  Interval in minutes to update player Sanguis.


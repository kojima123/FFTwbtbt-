/******************************************************
 サンプルプログラム ： １次元高速ウェーブレット変換
                           メインルーチン
 Copyright 1998,1999 H. Nakano
********************************************************/

#include <stdio.h>
#include <math.h>
#include "fwt.h"


void main(void){

   double s0[L];                    /* 入力信号 S0 */
   double s0r[L];                   /* 再構成信号 */
   double s1[L2],w1[L2];            /* 分解信号 s1, w1 */
   double p[K]={0.482962913145, 0.836516303738, 0.224143868042, -0.129409522551}; 
            /* ドベシイの数列 p_k (N=2) */
   double q[K];  /* ドベシイの数列 q_k (N=2) */
 
   int i;
   int sup=K,s_len=L;

	for(i=0;i<L;i++){  /* s0 の初期値設定 */
		s0[i]=0.0;
	}
	for(i=0;i<16;i++){  
		s0[i]=(float)((i+1)*(i+1))/256.0;
	}
	for(i=16;i<32;i++){
		s0[i]=.2;
	}
	for(i=32;i<48;i++){
		s0[i]=(float)((47-i)*(47-i))/256.0-.5;
	}


   for(i=0;i<sup;i++){  /* p_k から q_k を生成 */
      q[i]=pow(-1,i)*p[sup-i-1];
	}

	printf("入力信号 \n");
	for(i=0;i<L;i++){
		printf("%7.4f ",s0[i]);
	}
	printf("\n");
	
   fwt1d(s0,s_len,p,q,sup,s1,w1);  /* 1次元高速ウェーブレット変換 */
	
   ifwt1d(s1,w1,s_len/2,p,q,sup,s0r);  /* 1次元高速ウェーブレット逆変換 */

	printf("再構成信号 \n");
	for(i=0;i<L;i++){
		printf("%7.4f ",s0r[i]);
	}
	printf("\n");
}